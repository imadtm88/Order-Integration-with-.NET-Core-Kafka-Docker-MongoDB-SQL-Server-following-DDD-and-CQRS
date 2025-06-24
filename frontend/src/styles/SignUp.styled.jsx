// SignUp.styled.jsx
import styled, { keyframes } from 'styled-components';
import { Box } from "@chakra-ui/react";

export const SignUpWrapper = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 100%;
  height: 100vh;
  background-position: center center;
  background-repeat: no-repeat;
  background-size: cover;
  position: relative;
  overflow-x: hidden; // Prevent horizontal scrolling
`;

export const SignUpBackdrop = styled.div`
  position: absolute;
  inset: 0;
  background-color: rgba(0, 0, 0, 0.3);
`;

const showSignUpForm = keyframes`
  0%,
  30% {
    transform: translate(0, -150%);
  }
  70%,
  90% {
    transform: translate(0, 1rem);
  }
  80%,
  100% {
    transform: translate(0, 0);
  }
`;

export const FormContainer = styled(Box)`
  width: 90%;
  max-width: 24rem;
  z-index: 1;
  animation: ${showSignUpForm} 1s;
  @media (max-width: 480px) {
    width: 100%;
    padding: 1rem;
  }
`;

export const LogoImage = styled.img`
  width: 9rem;
  background-position: center center;
`;

export const FooterText = styled.div`
  width: 100%;
  margin-bottom: 2rem;
  position: absolute;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  text-align: center;
  color: white;
`;
