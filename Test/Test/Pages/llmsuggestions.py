from langchain_openai import OpenAIEmbeddings
from pinecone import Pinecone


def search(query):
    pc = Pinecone(api_key="62226ed0-3134-4bd8-a961-8c3d9ae26e7c")
    embeddings = OpenAIEmbeddings(model="text-embedding-3-small", dimensions=1536,
                                  openai_api_key='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')
    index = pc.Index("smart-search-box-dotproduct-newitemswithdesc1")
    query_vector = embeddings.embed_query(query)
    result = index.query(
        vector=query_vector,
        top_k=3,
        include_values=False,
        namespace='products'
    )
    ids = [int(match['id']) for match in result['matches']]
    return ids
