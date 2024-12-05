import { useState } from 'react';
import { toast } from 'react-toastify';
import { uploadCountryImage } from '../Services/countryService';

export const useUploadCountryImage = () => {
  const [countryImages, setCountryImages] = useState({});

  const handleUploadCountryImage = async (file, countryId) => {
    try {
      await uploadCountryImage(file, countryId);
      setCountryImages((prev) => ({ ...prev, [countryId]: file }));
      toast.success('Image uploaded successfully!');
    } catch (error) {
      toast.error('Error uploading file: ' + error.message);
    }
  };

  return {
    handleUploadCountryImage,
    countryImages,
    setCountryImages,
  };
};
