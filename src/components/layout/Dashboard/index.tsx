import React from "react";
import Shell from "@/layout/Shell";
import { Wrapper, Container, TopSection, ContentSection } from "./Wrappers";
import Button from "@/common/Button";
import Text from "@/common/Text";

const Dashboard = () => (
  <Shell>
    <Wrapper>
      <Container>
        <TopSection>
          <Text>Resources</Text>
          <Button variant="primary">Assign Job</Button>
        </TopSection>
        <ContentSection>content</ContentSection>
      </Container>
    </Wrapper>
  </Shell>
);

export default Dashboard;
