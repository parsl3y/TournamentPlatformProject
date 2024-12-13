import React from 'react';

const PaginationControls = ({ currentPage, hasMoreGames, setPageAction }) => {
  const handleNextPage = () => {
    if (hasMoreGames) {
      setPageAction(currentPage + 1);
    }
  };

  const handlePreviousPage = () => {
    setPageAction(Math.max(currentPage - 1, 1));
  };

  return (
    <div className="pagination">
      <button onClick={handlePreviousPage} disabled={currentPage === 1}>
        Previous
      </button>
      <span>Page {currentPage}</span>
      <button onClick={handleNextPage} disabled={!hasMoreGames}>
        Next
      </button>
    </div>
  );
};

export default PaginationControls;
