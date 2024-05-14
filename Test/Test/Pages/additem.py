from langchain_openai import OpenAIEmbeddings
from pinecone import Pinecone


def add(name, description, itemid):
    embeddings = OpenAIEmbeddings(model="text-embedding-3-small", dimensions=1536,
                                  openai_api_key='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')
    pc = Pinecone(api_key="62226ed0-3134-4bd8-a961-8c3d9ae26e7c")
    index = pc.Index("smart-search-box-dotproduct-newitemswithdesc1")
    embedded_data = embeddings.embed_query(name + " " + description)
    print(embedded_data)
    vectors=[]
    vectors.append({"id": str(itemid), "values": embedded_data})
    index.upsert(ids=[str(itemid)], vectors=vectors, namespace='products')
