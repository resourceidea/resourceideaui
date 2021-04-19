import styled from "styled-components";

export const Wrapper = styled.div`
  height: calc(100vh - 57px);
  background-color: #f9f9f9;
  min-height: 100%;
  padding: 28px 20px;
`;

export const Container = styled.div`
  overflow: hidden;
  height: calc(100vh - 57px - 56px);
  min-height: 100%;
  background-color: white;
  box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.05);
  border-radius: 4px;
  display: flex;
  flex-direction: column;
`;

export const TopSection = styled.div`
  padding: 24px;
  border-bottom: 1px solid #f1f3f9;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

export const ContentSection = styled.div`
  flex-grow: 1;
  overflow: scroll;
  /* width */
  ::-webkit-scrollbar {
    display: none;
  }

  /* Track */
  ::-webkit-scrollbar-track {
    display: none;
  }
`;
