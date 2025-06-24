// import React, { useState } from "react";
// import axios from "axios";
// import { Box, Button, Input, VStack, Heading, FormControl, FormLabel, Alert, AlertIcon } from "@chakra-ui/react";
// import { useAuth } from '../Auth/AuthContext';

// const SignUp = () => {
//   const [username, setUsername] = useState("");
//   const [password, setPassword] = useState("");
//   const [email, setEmail] = useState("");
//   const [error, setError] = useState("");
//   const [success, setSuccess] = useState("");
//   const { login } = useAuth();

//   const handleSignUp = async () => {
//     try {
//       const response = await axios.post("http://localhost:5109/api/Auth/signup", {
//         username,
//         password,
//         email,
//       });
//       if (response.data.success) {
//         setSuccess("Sign up successful!");
//         setError("");
//         login(); // Authentifier l'utilisateur après inscription
//       } else {
//         setError(response.data.message);
//       }
//     } catch (err) {
//       setError("An error occurred during sign up.");
//     }
//   };

//   return (
//     <Box p={4}>
//       <VStack spacing={4}>
//         <Heading>Sign Up</Heading>
//         {error && (
//           <Alert status="error">
//             <AlertIcon />
//             {error}
//           </Alert>
//         )}
//         {success && (
//           <Alert status="success">
//             <AlertIcon />
//             {success}
//           </Alert>
//         )}
//         <FormControl id="username">
//           <FormLabel>Username</FormLabel>
//           <Input
//             type="text"
//             value={username}
//             onChange={(e) => setUsername(e.target.value)}
//           />
//         </FormControl>
//         <FormControl id="email">
//           <FormLabel>Email</FormLabel>
//           <Input
//             type="email"
//             value={email}
//             onChange={(e) => setEmail(e.target.value)}
//           />
//         </FormControl>
//         <FormControl id="password">
//           <FormLabel>Password</FormLabel>
//           <Input
//             type="password"
//             value={password}
//             onChange={(e) => setPassword(e.target.value)}
//           />
//         </FormControl>
//         <Button colorScheme="teal" onClick={handleSignUp}>
//           Sign Up
//         </Button>
//       </VStack>
//     </Box>
//   );
// };

// export default SignUp;


import React, { useState } from "react";
import axios from "axios";
import { VStack, Heading, FormControl, FormLabel, Input, Button, Alert, AlertIcon, Image, Text } from "@chakra-ui/react";
import { useAuth } from '../Auth/AuthContext';
import BackgroundImage from "../assets/ccc.jpg";
import Logo from "../assets/logo.png";
import { SignUpWrapper, SignUpBackdrop, FormContainer, LogoImage, FooterText } from '../styles/SignUp.styled';

const SignUp = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const { login } = useAuth();

  const handleSignUp = async () => {
    try {
      const response = await axios.post("http://localhost:5109/api/Auth/signup", {
        username,
        password,
        email,
      });
      if (response.data.success) {
        setSuccess("Sign up successful!");
        setError("");
        login(); // Authentifier l'utilisateur après inscription
      } else {
        setError(response.data.message);
      }
    } catch (err) {
      setError("An error occurred during sign up.");
    }
  };

  return (
    <SignUpWrapper style={{ backgroundImage: `url(${BackgroundImage})` }}>
      <SignUpBackdrop />
      <FormContainer className="shadow p-4 bg-white rounded" as="form" onSubmit={(e) => { e.preventDefault(); handleSignUp(); }}>
        <LogoImage className="img-thumbnail mx-auto d-block mb-2" src={Logo} alt="logo" />
        <Heading as="h4" mb={2} textAlign="center">Sign Up</Heading>
        {error && (
          <Alert status="error" mb={2}>
            <AlertIcon />
            {error}
          </Alert>
        )}
        {success && (
          <Alert status="success" mb={2}>
            <AlertIcon />
            {success}
          </Alert>
        )}
        <VStack spacing={4}>
          <FormControl id="username" isRequired>
            <FormLabel>Username</FormLabel>
            <Input
              type="text"
              value={username}
              placeholder="Username"
              onChange={(e) => setUsername(e.target.value)}
            />
          </FormControl>
          <FormControl id="email" isRequired>
            <FormLabel>Email</FormLabel>
            <Input
              type="email"
              value={email}
              placeholder="Email"
              onChange={(e) => setEmail(e.target.value)}
            />
          </FormControl>
          <FormControl id="password" isRequired>
            <FormLabel>Password</FormLabel>
            <Input
              type="password"
              value={password}
              placeholder="Password"
              onChange={(e) => setPassword(e.target.value)}
            />
          </FormControl>
          <Button colorScheme="teal" type="submit" w="100%">
            Sign Up
          </Button>
        </VStack>
      </FormContainer>
      <FooterText>
        Made by Cdiscount C | &copy;2022
      </FooterText>
    </SignUpWrapper>
  );
};

export default SignUp;
