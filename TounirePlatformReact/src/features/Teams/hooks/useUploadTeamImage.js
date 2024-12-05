import { useState } from 'react';
import { uploadTeamImage } from '../Services/teamService';

export const useUploadTeamImage = () => {
  const [teamImages, setTeamImages] = useState({});

  const handleUploadTeamImage = async (file, teamId) => {
    try {
      await uploadTeamImage(file, teamId);
      setTeamImages((prev) => ({ ...prev, [teamId]: file }));
    } catch (error) {
      toast.error('Error uploading file: ' + error.message);
    }
  };

  return {
    handleUploadTeamImage,
    teamImages,
    setTeamImages
  };
};