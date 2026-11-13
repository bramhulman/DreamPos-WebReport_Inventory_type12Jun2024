"""Repair only malformed column metadata using an explicit reference MySQL dump.
Does not connect to or change any database. Valid master definitions are preserved.
"""
import argparse
import json
import re
from pathlib import Path


def split_definitions(text):
    chunks, start, depth, quote, i = [], 0, 0, None, 0
    while i < len(text):
        c = text[i]
        if quote:
            if c == '\\':
                i += 2
                continue
            if c == quote:
                if i + 1 < len(text) and text[i + 1] == quote:
                    i += 2
                    continue
                quote = None
        elif c in "'\"`":
            quote = c
        elif c == '(':
            depth += 1
        elif c == ')':
            depth -= 1
        elif c == ',' and depth == 0:
            chunks.append(text[start:i].strip())
            start = i + 1
        i += 1
    chunks.append(text[start:].strip())
    return chunks, quote is None and depth == 0


def column_type(definition):
    match = re.match(r'[a-zA-Z]+', definition)
    if not match:
        raise ValueError('Missing type: ' + definition)
    end = match.end()
    if end < len(definition) and definition[end] == '(':
        quote, depth, i = None, 0, end
        while i < len(definition):
            c = definition[i]
            if quote:
                if c == '\\':
                    i += 2
                    continue
                if c == quote:
                    if i + 1 < len(definition) and definition[i + 1] == quote:
                        i += 2
                        continue
                    quote = None
            elif c in "'\"":
                quote = c
            elif c == '(':
                depth += 1
            elif c == ')':
                depth -= 1
                if depth == 0:
                    end = i + 1
                    break
            i += 1
        else:
            raise ValueError('Unclosed type: ' + definition)
    tail = re.match(r'(?:\s+(?:unsigned|zerofill|binary))*(?=\s|$)', definition[end:], re.I)
    return definition[:end + (tail.end() if tail else 0)].strip()


def repair(master_path, dump_path):
    raw = dump_path.read_bytes()
    dump = raw.decode('utf-16' if raw.startswith((b'\xff\xfe', b'\xfe\xff')) else 'utf-8-sig')
    reference = {}
    for match in re.finditer(r'CREATE TABLE `((?:``|[^`])+)`\s*\((.*?)\)\s*ENGINE=.*?;', dump, re.S):
        chunks, valid = split_definitions(match[2])
        if not valid:
            raise ValueError('Unbalanced reference table: ' + match[1])
        columns = {}
        for chunk in chunks:
            col = re.match(r'^`((?:``|[^`])+)`\s+(.+)$', chunk, re.S)
            if col:
                columns[col[1].replace('``', '`')] = col[2]
        reference[match[1].replace('``', '`')] = columns
    master = json.loads(master_path.read_text(encoding='utf-8-sig'))
    changes = []
    for table, data in master['tables'].items():
        for name, col in list(data['columns'].items()):
            typ = col.get('type', '')
            if re.match(r'^(?:KEY|PRIMARY|UNIQUE|FULLTEXT|SPATIAL|CONSTRAINT|FOREIGN|CHECK)(?:\s|$)', typ, re.I):
                del data['columns'][name]
                changes.append({'table': table, 'column': name, 'action': 'remove-index-pseudocolumn'})
                continue
            if not split_definitions(typ)[1] or not split_definitions(col.get('definition', ''))[1]:
                source = reference.get(table, {}).get(name)
                if source is None:
                    raise ValueError('No reference for invalid column ' + table + '.' + name)
                source_type = column_type(source)
                if not source_type.lower().startswith(typ.lower()):
                    raise ValueError('Reference type differs; manual review required: ' + table + '.' + name)
                changes.append({'table': table, 'column': name, 'action': 'repair-malformed-definition', 'old': col['definition'], 'new': source})
                col['type'] = source_type
                col['definition'] = source
    return master, changes


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--master', type=Path, required=True)
    parser.add_argument('--reference-dump', type=Path, required=True)
    parser.add_argument('--output', type=Path, required=True)
    parser.add_argument('--report', type=Path, required=True)
    args = parser.parse_args()
    master, changes = repair(args.master, args.reference_dump)
    args.output.write_text(json.dumps(master, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    args.report.write_text(json.dumps(changes, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print('Repaired', sum(x['action'] == 'repair-malformed-definition' for x in changes), 'definitions; removed', sum(x['action'] == 'remove-index-pseudocolumn' for x in changes), 'index pseudocolumns.')
