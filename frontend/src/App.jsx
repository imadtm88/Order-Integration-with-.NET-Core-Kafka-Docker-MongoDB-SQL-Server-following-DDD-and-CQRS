import React from "react";
import { Outlet } from "react-router-dom";
import { Navbar } from "./components/Navbar";
import { ChakraProvider } from "@chakra-ui/react";
import GlobalStyles from "./styles/Global.styled";

function App() {
  return (
    <ChakraProvider>
      <div className="App">
        <Navbar />
        <Outlet />
        <GlobalStyles />
      </div>
    </ChakraProvider>
  );
}

export default App;