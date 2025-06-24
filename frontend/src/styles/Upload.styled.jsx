import styled from 'styled-components';
import { Link } from 'react-router-dom';

export const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 20px;
  box-sizing: border-box;
  background-color: #f9f9f9;
`;

export const UploadBox = styled.div`
  border: 2px dashed #b3d7ff;
  padding: 20px;
  text-align: center;
  border-radius: 10px;
  background-color: white;
  margin-bottom: 20px;
  width: 100%;
  max-width: 500px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
`;

export const UploadIcon = styled.div`
  font-size: 24px;
  color: #3b82f6;
  margin-bottom: 10px;
`;

export const UploadText = styled.div`
  margin-bottom: 10px;
`;

export const UploadOr = styled.div`
  margin-bottom: 10px;
`;

export const UploadButton = styled.label`
  background-color: #3b82f6;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  margin-top: 10px;
  margin-bottom: 10px;
`;

export const UploadSupported = styled.div`
  margin-top: 10px;
  color: #a1a1a1;
  font-size: 14px;
`;

export const UploadedFiles = styled.div`
  width: 100%;
  max-width: 500px;
  margin-top: 20px;
`;

export const UploadedFilesTitle = styled.div`
  font-weight: bold;
  margin-bottom: 10px;
  text-align: center;
`;

export const UploadedFile = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 10px;
  padding: 10px;
  background-color: white;
  border-radius: 5px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
`;

export const FileIcon = styled.div`
  width: 24px;
  height: 24px;
  margin-right: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
`;

export const FileName = styled.div`
  flex-grow: 1;
  font-size: 14px;
`;

export const FileActions = styled.div`
  display: flex;
  align-items: center;
`;

export const FileDelete = styled.div`
  cursor: pointer;
  color: red;
  margin-left: 10px;
`;

export const FileDetails = styled(Link)`
  cursor: pointer;
  color: #3b82f6;
  text-decoration: none;
  
  &:hover {
    text-decoration: underline;
  }
`;

export const ConfirmationButton = styled.button`
  margin-top: 20px;
  padding: 10px 20px;
  font-size: 16px;
  color: white;
  background-color: #007bff;
  border: none;
  border-radius: 5px;
  cursor: pointer;

  &:hover {
    background-color: #0056b3;
  }
`;