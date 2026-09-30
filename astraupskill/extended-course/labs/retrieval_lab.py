"""ASCII/numeric-ID teaching model, not a .NET runtime or persistence test."""
import itertools
import re
STOP=set('a an and for in of on or the to with incident investigate service'.split())
def tokens(value): return re.findall('[a-z0-9]+',value.lower())
def search(documents,query,top_k=3,minimum=3):
    if top_k<=0 or minimum<=0: return []
    terms=list(dict.fromkeys(t for t in tokens(query) if t not in STOP))
    if not terms: return []
    result=[]
    for identifier,title,tags,content in documents:
        fields=[set(tokens(x)) for x in (title,tags,content)]
        score=sum(sum(weight for field,weight in zip(fields,(4,3,1)) if term in field) for term in terms)
        if score>=minimum: result.append((identifier,title,score))
    return sorted(result,key=lambda x:(-x[2],x[1].lower(),x[0]))[:top_k]
def checks():
    docs=[(1,'Queue Recovery','operations','inspect queue depth'),(2,'Worker Guide','queue','worker guide'),(3,'Generic Guide','general','queue')]
    expected=[(1,'Queue Recovery',5),(2,'Worker Guide',3)]
    assert search(docs,'queue')==expected, 'field weights and inclusive threshold'
    assert search(docs,'queue queue')==expected, 'query repetition does not boost'
    assert search(docs,'incident service investigate')==[], 'empty filtered query'
    assert search(docs,'queue',top_k=1)==expected[:1], 'bounded result'
    assert search(docs,'queue',top_k=0)==[] and search(docs,'queue',minimum=0)==[], 'nonpositive bounds'
    tied=[(5,'Same','queue',''),(4,'Same','queue','')]
    for order in itertools.permutations(tied):
        assert [x[0] for x in search(order,'queue')]==[4,5], 'fixture tie ordering'
    repeated=[(1,'Queue Recovery','operations','queue queue queue')]+docs[1:]
    assert search(repeated,'queue')==expected, 'field occurrence repetition does not boost'
    print('7 retrieval-model properties passed; ASCII fixture only')
    for row in expected: print(row)
if __name__=='__main__': checks()
