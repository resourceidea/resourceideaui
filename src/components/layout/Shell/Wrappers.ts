import styled from "styled-components";

const sidebarWidth = "300px";

export const Wrapper = styled.div`
  width: 100%;
  min-height: 768px;
  height: 100vh;
  display: flex;
  flex-wrap: wrap;
`;

export const Sidebar = styled.div`
  width: ${(props: { open: boolean }) => (props.open ? "300px" : "48px")};
  min-height: 768px;
  height: 100vh;
  background-color: #005866;
  display: flex;
`;

export const SidePanel = styled.div`
  width: ${sidebarWidth};
  min-height: 768px;
  height: 100vh;
  width: 48px;
  background-color: #003d47;
`;

export const Mainbar = styled.div`
  flex-grow: 1;
  min-height: 768px;
  height: 100vh;
`;

export const ContentBar = styled.div`
  height: calc(100vh - 57px);
  width: 100%;
  overflow-y: hidden;
`;
