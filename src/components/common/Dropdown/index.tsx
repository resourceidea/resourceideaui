import React from 'react'
import Portal from "@reach/portal";
import styled from 'styled-components'

const DropdownWrapper = styled.div<{ top?: number, left?: number }>`
  padding: 30px;
  background-color: #333;
  color: white;
  position: absolute;
  top: ${props => props.top}px;
  left: ${props => props.left}px;
  width: 200px;
  z-index: 1000000;
`;

const Dropdown: React.FC<{ target: React.ReactNode }> = ({ target, children }) => {
  const targetRef = React.useRef(null);
  return (
    <>
      <div ref={targetRef}>
        {target}
      </div>
      <Portal>
        <DropdownWrapper >
          {children}
        </DropdownWrapper>
      </Portal>
    </>
  )
}

export default Dropdown
