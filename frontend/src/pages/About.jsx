import React from "react";
import {
  PageContainer,
  LeftWay,
  RightWay,
  Title,
  Paragraph,
  Footer,
  FooterContainer,
  GlobalStyle
} from "../styles/About.styled";

const About = () => {
  return (
    <>
      <GlobalStyle /> {/* Apply global styles */}
      <PageContainer>
        <LeftWay />
        <RightWay>
          <Title>About Us</Title>
          <Paragraph>
            Cdiscount est connu pour offrir des prix compétitifs et des promotions régulières. Ils ont également développé divers services, tels que la livraison à domicile, la location de véhicules, la billetterie en ligne, et même des offres d'énergie et de téléphonie mobile. L'entreprise a connu une croissance significative au fil des ans et est devenue l'un des principaux acteurs du commerce électronique en France. Cdiscount est également présent dans d'autres pays, notamment en Belgique, au Sénégal, en Côte d'Ivoire, au Cameroun, au Bénin, au Congo, au Gabon, au Mali, au Burkina Faso, au Togo, et au Vietnam.
          </Paragraph>
          <FooterContainer>
            <Footer>
              <p>&copy; Marketplace, Services complémentaires & E-commerce</p>
              <p>Cdiscount</p>
            </Footer>
          </FooterContainer>
        </RightWay>
      </PageContainer>
    </>
  );
};

export default About;
