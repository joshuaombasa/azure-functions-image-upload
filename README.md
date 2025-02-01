# Azure Functions Image Upload

This project demonstrates an Azure Functions solution that allows users to upload images to Azure Blob Storage. The function validates and processes the image asynchronously using Azure Queue Storage for background processing.

## Features:
- HTTP-triggered image upload.
- File validation and Blob Storage integration.
- Background image processing (e.g., resizing, watermarking).
- CI/CD integration using GitHub Actions.

## Setup:
1. Clone the repository.
2. Set up an Azure Function App.
3. Add your Azure Blob Storage connection string to the `local.settings.json`.
4. Deploy using GitHub Actions.

## Testing:
- Use Postman or similar tools to send a `POST` request with an image file to the function URL.
