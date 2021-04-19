import React from "react";
import styled from "styled-components";

const Wrapper = styled.div`
  z-index: 2000;
  position: relative;
  height: 57px;
  width: 100%;
  box-shadow: 0px 1px 0px rgba(127, 162, 168, 0.3);
  display: flex;
  align-items: center;
  padding-left: 24px;
  padding-right: 24px;
`;

type NavProps = {
  isOpen?: boolean;
  setOpen?: Function;
};
const Navbar = ({ setOpen }: NavProps) => {
  const toggleSidebar = (e) => {
    e.preventDefault();
    setOpen((open) => !open);
  };
  return (
    <Wrapper>
      <button onClick={toggleSidebar}>Toggle</button>
      Navbar
    </Wrapper>
  );
};

export default Navbar;
