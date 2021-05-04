import styled from "styled-components";

export const sidebarWidth = "300px";

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
