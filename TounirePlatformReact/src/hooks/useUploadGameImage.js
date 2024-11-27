import { useState } from 'react';
import { uploadGameImage } from '../features/Games/Services/gameService';

export const useUploadGameImage = () => {
  const [gameImages, setGameImages] = useState({});

  const handleUploadGameImage = async (file, gameId) => {
    try {
      await uploadGameImage(file, gameId);
      setGameImages((prev) => ({ ...prev, [gameId]: file }));
    } catch (error) {
      alert('Error uploading file: ' + error.message);
    }
  };

  return {
    handleUploadGameImage,
    gameImages,
    setGameImages
  };
};
