# DocumentationChatFriend

Ett lokalt RAG (Retrieval-Augmented Generation) API byggt med ASP.NET Core (.NET 9) och Clean Architecture. Allt du behöver är Docker – inga ytterligare beroenden eller konfigurationer krävs lokalt.

## 🚀 Kom igång

### Förkrav
- [Docker](https://www.docker.com/) installerat

### Starta projektet
Ladda ner docker-compose filen, och öppna en terminal i samma mapp där filen finns.
Skriv in kommandot:
```bash
docker compose up
```
Detta startar API:t tillsammans med Ollama och Qdrant, och laddar automatiskt ned alla nödvändiga modeller från Docker Hub. I terminalen kommer du se hur nödvändiga Ollama modeller laddas ned. När nedladdningen är färdig startar API:et automatiskt igång.

### Modellkonfiguration

Du kan konfigurera vilka modeller som används direkt i docker-compose.yml genom att sätta miljövariabler:

I docker-compose filen ser det ut så här:
```
  documentation-chat-friend-backend:
    image: josephrashidmaalouf/documentation-chat-friend-backend:latest
    container_name: "documentation-chat-friend-backend"
    ports:
      - "5143:5143"
    environment:
      - DOTNET_ENVIRONMENT=Docker
      - OllamaModelConfigs__Models__0=gemma3:1b 
      - OllamaModelConfigs__Models__1=nomic-embed-text:latest # This line, and the one above will make sure these models are pulled to the ollama container
      - OllamaClientConfigs__Model=gemma3:1b # This sets the LLM to be used for formulating answers
      - OllamaClientConfigs__MaxTokens=512 # Configure max tokens allowed in response
      - OllamaClientConfigs__Temperature=0.9 # Configure temperature (creativity) in responses. A lower number means less creative
      - VectorRepositoryConfigs__MinScore=0.7 # Configure the accuracy score on the embedding retrieved from the database in relation to the question asked
      - VectorRepositoryConfigs__Limit=3 # Configure maximum allowed embeddings to be retrieved from the database
    pull_policy: always

```
- OllamaClientConfigs__Model=gemma3:1b 
På den raden kan du byta ut värdet till valfri lokal Ollama model. Men se då till att den laddas ned genom att lägga till en ny rad för Models:
`- OllamaModelConfigs__Models__3=ny-model `
Eller byta ut Models på index 0 till den modell du vill använda.

För en lista på Ollama modeller hänvisar jag till deras dokumentation: [https://ollama.com/library](https://ollama.com/library)
Se till att inte använda en starkare modell än vad din maskin klarar av. Tar modellen med än 100 sekunder på sig att formulera ett svar så kommer API:t inte svara.

Just nu är embedding modellen inte konfigurerbar, men det kommer i nästa version.

Här är teknikstack-sektionen i kopierbar `.md`-format:

## Teknikstack och arkitektur

* .NET 9 (ASP.NET Core)
* Clean Architecture
* Ollama (för både embedding och LLM-svar)
* Qdrant (vektordatabas)
* Docker Compose

## Nuvarande funktioner

* Två endpoints:

  * `POST /api/upload` för att ladda in och embeda text
  * `POST /api/completions` för att ställa frågor och få svar baserat på embedda data
* Använder Ollama embedding modeller (default: `nomic-embed-text`)
* Lagrar embeddingar i Qdrant
* Använder Ollama LLM (default: `gemma3:1b`) för att generera svar baserat på relevant fakta
* Inga hallucinationer – svarar "Jag vet inte" om ingen fakta hittas
* Docker Compose för enkel lokal körning utan extra konfiguration

## Planerade features

* Konfigurerbar embedding modell
* Endpoint för chatt med kontext/minne
* Streaming av svar från LLM
* Möjlighet att läsa in `.txt` och `.pdf`-filer direkt
* Frontend-gränssnitt

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
