import React from "react";
import { Wrapper, Container, TopSection, ContentSection } from "./Wrappers";
import Button from "@/common/Button";
import Text from "@/common/Text";

const Dashboard = () => (
  <Wrapper>
    <Container>
      <TopSection>
        <Text>Resources</Text>
        <Button variant="primary">Assign Job</Button>
      </TopSection>
      <ContentSection>content</ContentSection>
    </Container>
  </Wrapper>
);

export default Dashboard;
