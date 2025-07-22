# DocumentationChatFriend

# API Documentation

## Endpoints

### POST `/api/completions`

Submit a question to the RAG (Retrieval-Augmented Generation) service and receive a generated answer.

**Request Body**

```
{
    "question" : "string",
    "collectionName" : "string"
}
```

- `question` (string, required): The question to be answered.
- `collectionName` (string, required): The name of the collection to query facts from.

**Responses**

- `200 OK`: Returns the generated answer as a string.
- `404 Not Found`: The specified collection or data was not found.
- `503 Service Unavailable`: The service is temporarily unavailable or an error occurred.

---

### POST `/api/upload`

Upload text to a collection, chunked according to the specified style, for later querying.

**Request Body**

```
{
    "collectionName" : "string,
    "text" : string,
    "chunkingStyle" 0|1|2
}
```

- `collectionName` (string, required): The name of the collection to upload to.
- `text` (string, required): The text content to be chunked and uploaded.
- `chunkingStyle` (enum, required): The chunking style. Possible values: 0 = `Sentence`, 1 = `Paragraph`, 2 = `Custom`.

**Query Parameters**

- `chunkLength` (int, optional, default: 10): (Custom only) Length of each chunk.
- `overlap` (int, optional, default: 0): (Custom only) Overlap between chunks.

Will only be used with the Custom (2) chunkingStyle

**Responses**

- `200 OK`: Upload and chunking succeeded.
- `503 Service Unavailable`: The service is temporarily unavailable or an error occurred.

---
