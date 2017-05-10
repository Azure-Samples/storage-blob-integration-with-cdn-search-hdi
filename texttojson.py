def txtToJson (filename, text):
    import json, re, os
    fields = dict(
            map(lambda (key, val): (key.replace(' ', '').strip(), val.strip()),
                map(lambda clean: clean.partition(":")[0:3:2],
                    map(lambda field: re.sub(r"\s{2,}", ' ', field),
                        re.split(r"\r\n\r", text)))))
    fields.pop('', None)
    fields["FileName"] = os.path.split(filename)[1]
    fields["MyURL"] = "https://azure.storagedemos.com/clinical-trials/" + os.path.split(filename)[1]
    return json.dumps(fields)

def jsonToJsonArray (iterator):
    yield '['
    prev = iterator.next()
    for item in iterator:
        yield prev + ','
        prev = item
    yield prev + ']'

(sc.wholeTextFiles("wasbs:///clinical-trials", minPartitions=8)
 .map(lambda (filename, text): txtToJson(filename, text))
 .mapPartitions(lambda iterator: jsonToJsonArray(iterator))
 .saveAsTextFile("wasbs:///clinical-trials-json"))