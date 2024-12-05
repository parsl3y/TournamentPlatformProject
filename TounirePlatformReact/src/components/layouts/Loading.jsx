import React, { useState, useEffect } from 'react';
import './Loading.css'; 

const Loading = () => {
  const [isSpinnerVisible, setIsSpinnerVisible] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => {
      setIsSpinnerVisible(false); 
    }, 5000); 

    return () => clearTimeout(timer);
  }, []);

  return (
    <div className="spinner-container">
      {isSpinnerVisible ? (
        <div className="spinner"></div> 
      ) : (
        'Loading...' 
      )}
    </div>
  );
};

export default Loading;
