import React from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import App from "./App";
import Home from "./pages/Home";
import About from "./pages/About";
import Upload from "./pages/Upload";
import SignUp from "./pages/SignUp";
import Table from "./pages/Table";
import ProtectedRoute from "./Auth/ProtectedRoute";
import { AuthProvider } from "./Auth/AuthContext";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "",
        element: <Home />,
      },
      { path: "about", element: <About /> },
      { path: "upload", element: <ProtectedRoute><Upload /></ProtectedRoute> },
      { path: "sign-up", element: <SignUp /> },
      { path: "table", element: <ProtectedRoute><Table /></ProtectedRoute> },
    ],
  },
]);

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  </React.StrictMode>
);
