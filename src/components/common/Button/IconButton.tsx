import React, { FunctionComponent, ReactEventHandler } from 'react'
import styled from "styled-components";

const StyledButton = styled.button`
  appearance: none;
  outline: none;
  margin: 0;
  border: none;
  cursor: pointer;
  font-size: 0.75rem !important;
  font-weight: 600;
  padding: 4px !important;
  font-family: "DM Sans", san-serif !important;
  border-radius: 4px;
  text-transform: uppercase;
  background-color: transparent;
  svg {
    pointer-events: none;
  }
  `;

interface IconButtonProps {
  children: React.ReactChild;
  title: string;
  onClick?: ReactEventHandler;
}

const IconButton: FunctionComponent<IconButtonProps> = ({ children, onClick, title }) => {
  return (
    <StyledButton onClick={onClick} title={title}>
      {children}
    </StyledButton>
  )
}
export default IconButton;
