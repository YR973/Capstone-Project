from langchain_openai import OpenAIEmbeddings
from pinecone import Pinecone


def search(query):
    result = Pinecone(api_key="62226ed0-3134-4bd8-a961-8c3d9ae26e7c").Index("smart-search-box-dotproduct-newitemswithdesc1").query(
        vector=OpenAIEmbeddings(model="text-embedding-3-small", dimensions=1536,
                                openai_api_key='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').embed_query(query),
        top_k=20,
        include_values=False,
        namespace='products'
    )
    ids = [int(match['id']) for match in result['matches']]
    return ids
