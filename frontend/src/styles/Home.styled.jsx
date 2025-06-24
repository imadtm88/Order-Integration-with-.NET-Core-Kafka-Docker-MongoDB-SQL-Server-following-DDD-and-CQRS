import styled, { keyframes } from 'styled-components';

const fadeIn = keyframes`
  from {
    opacity: 0;
    transform: translate(-50%, -60%);
  }
  to {
    opacity: 1;
    transform: translate(-50%, -50%);
  }
`;

export const Banner = styled.div`
  height: 100vh;
  width: 100%;
  background: url("https://png.pngtree.com/thumb_back/fh260/background/20230616/pngtree-digital-3d-rendering-of-online-delivery-services-tracking-orders-and-logistics-image_3607141.jpg");
  background-position: top;
  background-size: cover;
  background-repeat: no-repeat;
`;

export const TextHome = styled.div`
  position: absolute;
  top: 50%;
  left: 51%;
  color: white;
  transform: translate(-50%, -50%);
  animation-name: ${fadeIn};
  animation-duration: 5s;
  animation-iteration-count: infinite;
`;





export const ButtonCSS = styled.button`
  --btn-color: #13bcdf;
  position: relative;
  padding: 16px 32px;
  font-family: Roboto, sans-serif;
  font-weight: 500;
  font-size: 16px;
  line-height: 1;
  left: 39%;
  color: white;
  background: none;
  border: none;
  outline: none;
  overflow: hidden;
  cursor: pointer;
  filter: drop-shadow(0 2px 8px rgba(39, 94, 254, 0.32));
  transition: 0.3s cubic-bezier(0.215, 0.61, 0.355, 1);

  &::before {
    position: absolute;
    content: "";
    top: 0;
    left: 0;
    z-index: -1;
    width: 100%;
    height: 100%;
    background: var(--btn-color);
    border-radius: 24px;
    transition: 0.3s cubic-bezier(0.215, 0.61, 0.355, 1);
  }

  span, span span {
    display: inline-flex;
    vertical-align: middle;
    transition: 0.3s cubic-bezier(0.215, 0.61, 0.355, 1);
  }

  span {
    transition-delay: 0.05s;
  }

  span:first-child {
    padding-right: 7px;
  }

  span span {
    margin-left: 8px;
    transition-delay: 0.1s;
  }

  ul {
    position: absolute;
    top: 50%;
    left: 0;
    right: 0;
    display: flex;
    margin: 0;
    padding: 0;
    list-style-type: none;
    transform: translateY(-50%);
  }

  ul li {
    flex: 1;
  }

  ul li a {
    display: inline-flex;
    vertical-align: middle;
    transform: translateY(55px);
    transition: 0.3s cubic-bezier(0.215, 0.61, 0.355, 1);
  }

  ul li a:hover {
    opacity: 0.5;
  }

  &:hover::before {
    transform: scale(1.2);
  }

  &:hover span, &:hover span span {
    transform: translateY(-55px);
  }

  &:hover ul li a {
    transform: translateY(0);
  }

  &:hover ul li:nth-child(1) a {
    transition-delay: 0.15s;
  }

  &:hover ul li:nth-child(2) a {
    transition-delay: 0.2s;
  }

  &:hover ul li:nth-child(3) a {
    transition-delay: 0.25s;
  }
`;
