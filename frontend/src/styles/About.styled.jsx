import styled, { createGlobalStyle } from "styled-components";

export const GlobalStyle = createGlobalStyle`
  body, html {
    margin: 0;
    padding: 0;
    height: 100%;
    overflow: hidden; /* Prevent scrolling */
  }
`;

export const PageContainer = styled.div`
  font-family: 'Poppins', sans-serif;
  margin: 0;
  padding: 0;
  height: 100vh;
  overflow: hidden;
  background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' version='1.1' xmlns:xlink='http://www.w3.org/1999/xlink' xmlns:svgjs='http://svgjs.com/svgjs' width='1440' height='560' preserveAspectRatio='none' viewBox='0 0 1440 560'%3e%3cg mask='url(%26quot%3b%23SvgjsMask1416%26quot%3b)' fill='none'%3e%3crect width='1440' height='560' x='0' y='0' fill='url(%23SvgjsRadialGradient1417)'%3e%3c/rect%3e%3cpath d='M39.245%2c126.185C69.658%2c125.773%2c98.742%2c111.118%2c113.485%2c84.514C127.825%2c58.638%2c123.781%2c26.986%2c108.13%2c1.881C93.4%2c-21.748%2c67.089%2c-34.42%2c39.245%2c-34.575C11.102%2c-34.732%2c-15.775%2c-22.575%2c-30.983%2c1.105C-47.548%2c26.898%2c-53.567%2c59.843%2c-38.56%2c86.572C-23.289%2c113.77%2c8.056%2c126.607%2c39.245%2c126.185' fill='rgba(11%2c 12%2c 12%2c 1)' class='triangle-float1'%3e%3c/path%3e%3cpath d='M169.48866705621646 150.68051512829433L259.26388079672364 43.69058162713182 152.27394729556113-46.08463211337536 62.498733555053946 60.90530138778715z' fill='rgba(11%2c 12%2c 12%2c 1)' class='triangle-float3'%3e%3c/path%3e%3cpath d='M513.1763730995391 62.9869624564983L590.4368952857263 99.0141356249716 605.8473939453427-34.89023423854596z' fill='rgba(11%2c 12%2c 12%2c 1)' class='triangle-float2'%3e%3c/path%3e%3cpath d='M1272.60770682498 151.51193425617828L1244.5875464522562 310.42216036077883 1403.4977725568567 338.4423207335025 1431.5179329295804 179.53209462890197z' fill='rgba(11%2c 12%2c 12%2c 1)' class='triangle-float1'%3e%3c/path%3e%3cpath d='M1115.567713364114 347.36070221653097L1091.6268404383509 436.7092563530297 1180.9753945748496 460.650129278793 1204.9162675006128 371.30157514229427z' fill='rgba(11%2c 12%2c 12%2c 1)' class='triangle-float2'%3e%3c/path%3e%3cpath d='M1125.3379095672224 279.8316020974489L1208.5877875942326 254.3795601569007 1183.1357456536844 171.1296821298905 1099.885867626674 196.58172407043872z' fill='rgba(11%2c 12%2c 12%2c 1)' class='triangle-float3'%3e%3c/path%3e%3c/g%3e%3cdefs%3e%3cmask id='SvgjsMask1416'%3e%3crect width='1440' height='560' fill='white'%3e%3c/rect%3e%3c/mask%3e%3cradialGradient cx='50%25' cy='50%25' r='772.53' gradientUnits='userSpaceOnUse' id='SvgjsRadialGradient1417'%3e%3cstop stop-color='rgba(39%2c 98%2c 23%2c 1)' offset='0'%3e%3c/stop%3e%3cstop stop-color='rgba(14%2c 42%2c 71%2c 1)' offset='1'%3e%3c/stop%3e%3cradialGradient%3e%3c/style%3e%3c/svg%3e");
`;

export const LeftWay = styled.div`
  height: 100vh;
  width: 50%;
  background: url("https://cdn.edi-static.fr/image/upload/c_scale,dpr_auto,f_auto,q_auto,w_auto/c_limit,w_auto/v1/Img/BREVE/2023/3/379552/Bordeaux-Aquitaine-France-2019-Cdiscount-logo-sign--F.jpg");
  background-position: top;
  background-size: cover;
  background-repeat: no-repeat;
  position: absolute;
  left: 0;
`;

export const RightWay = styled.div`
  height: 100vh;
  width: 50%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  position: absolute;
  right: 0;
  text-align: center;
  padding-bottom: 100px; /* Adjusted to make space for the footer */
`;

export const Title = styled.h1`
  color: black;
  font-size: 2.5rem; /* Increase font size */
  margin-bottom: 20px; /* Add space below the title */
`;

export const Paragraph = styled.p`
  color: black; /* Ensure the text color is black */
  max-width: 80%; /* Adjust the max width of the paragraph if necessary */
  margin: 0 auto; /* Center horizontally */
`;

export const FooterContainer = styled.div`
  position: absolute;
  bottom: 0;
  width: 100%;
  text-align: center;
  background-color: #fff; /* Add background color to footer */
  padding: 10px 0; /* Add padding to the footer */
`;

export const Footer = styled.footer`
  p {
    color: black;
    font-size: 80%;
    margin: 0;
  }
`;
