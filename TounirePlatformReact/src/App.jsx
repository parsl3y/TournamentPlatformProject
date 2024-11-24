import React from "react";
import { BrowserRouter as Router} from "react-router-dom";
import AppRouter from "./Components/routes/AppRouter";
const App = () => {
  return (
    <Router>
      <AppRouter />
    </Router>
  );
};

export default App;
