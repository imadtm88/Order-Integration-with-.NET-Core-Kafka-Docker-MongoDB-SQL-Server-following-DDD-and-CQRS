import React, { useState } from "react";
import axios from "axios";
import {Container,UploadBox,UploadIcon,UploadText,UploadOr,UploadButton,UploadSupported,UploadedFiles,UploadedFilesTitle,UploadedFile,FileIcon,FileName,FileActions,FileDelete,FileDetails,ConfirmationButton,} from "../styles/Upload.styled";

const Upload = () => {
  const [files, setFiles] = useState([]);
  const [uploadStatus, setUploadStatus] = useState(null);

  const handleFileUpload = async (event) => {
    const uploadedFiles = Array.from(event.target.files).map((file) => ({
      name: file.name,
      type: file.name.split(".").pop(),
      uploading: true,
      error: false,
    }));

    setFiles([...files, ...uploadedFiles]);

    const formData = new FormData();
    formData.append("file", event.target.files[0]);

    try {
      const response = await axios.post(
        "http://localhost:5159/api/upload",
        formData
      );

      if (response.status === 200) {
        const { message } = response.data;
        setUploadStatus({ success: true, message });
      } else {
        setUploadStatus({ success: false, message: "Échec de l'upload du fichier." });
      }
    } catch (error) {
      const errorMessage =
        error.response?.data?.message || "Une erreur s'est produite lors de l'upload du fichier.";
      setUploadStatus({ success: false, message: errorMessage });
      console.error("Erreur :", error);
    }
  };

  const handleDelete = (fileName) => {
    setFiles(files.filter((file) => file.name !== fileName));
  };

  const handleConfirmation = async () => {
    try {
      const response = await axios.post(
        "http://localhost:5159/api/save-files",
        { files }
      );

      if (response.status === 200) {
        console.log("Fichiers enregistrés avec succès !");
      }
    } catch (error) {
      console.error("Erreur lors de la sauvegarde des fichiers :", error);
    }
  };

  const getFileIcon = (fileType) => {
    switch (fileType) {
      case "psd":
        return "🖼️";
      case "pdf":
        return "📄";
      case "ai":
        return "🎨";
      case "xlsx":
      case "xls":
        return "📊";
      default:
        return "📁";
    }
  };

  return (
    <Container>
      <UploadBox>
        <UploadIcon>↑</UploadIcon>
        <UploadText>Drag and Drop files to upload</UploadText>
        <UploadOr>or</UploadOr>
        <UploadButton>
          Browse
          <input type="file" multiple onChange={handleFileUpload} style={{ display: "none" }} />
        </UploadButton>
        <UploadSupported>Supported Excel: xlsx, xltx, xlsm, xlsb</UploadSupported>
      </UploadBox>
      {uploadStatus && (
        <UploadedFiles>
          <UploadedFilesTitle>Upload Status</UploadedFilesTitle>
          <UploadedFile>
            {uploadStatus.success ? (
              <FileIcon>✅</FileIcon>
            ) : (
              <FileIcon>❌</FileIcon>
            )}
            <FileName>{uploadStatus.message}</FileName>
          </UploadedFile>
        </UploadedFiles>
      )}
      {files.length > 0 && (
        <UploadedFiles>
          <UploadedFilesTitle>Uploaded files</UploadedFilesTitle>
          {files.map((file, index) => (
            <UploadedFile key={index}>
              <FileIcon>{getFileIcon(file.type)}</FileIcon>
              <FileName>{file.name}</FileName>
              <FileActions>
                <FileDetails to={`/table`}>📊</FileDetails>
                <FileDelete onClick={() => handleDelete(file.name)}>🗑️</FileDelete>
              </FileActions>
            </UploadedFile>
          ))}
          <ConfirmationButton onClick={handleConfirmation}>Confirm Upload</ConfirmationButton>
        </UploadedFiles>
      )}
    </Container>
  );
};

export default Upload;