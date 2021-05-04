import React from "react";
import styled from "styled-components";
import useTheme from '@/lib/hooks/useTheme'
import { MenuIcon, ProfileIcon, ChevronIcon } from '@/common/icons'
import IconButton from '@/common/Button/IconButton'
import Dropdown from '@/common/Dropdown'

type NavProps = {
  isOpen?: boolean;
  setOpen: Function;
};


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
  display:flex;
  justify-content: space-between;
`;
const RightPane = styled.div``;

const ProfileButton = styled.button`
  cursor: pointer;
  border: none;
  outline: none;
  background: ${(props) => props.theme.colors.navy[2]};
  padding: 7px 9px;
  border-radius: 200px;
  width: 60px;
  position: relative;
  display: flex !important;
  justify-content: space-between;
  align-items: center;
  &:hover {
    filter: brightness(0.95)
  }
`;

const Navbar = ({ setOpen }: NavProps) => {
  const theme = useTheme()
  const toggleSidebar = (e: React.SyntheticEvent) => {
    e.preventDefault();
    setOpen((open: boolean) => !open);
  };
  return (
    <Wrapper>
      <IconButton onClick={toggleSidebar} title="Menu button"><MenuIcon /></IconButton>
      <RightPane>
        <Dropdown
          target={
            <ProfileButton>
              <ProfileIcon fill={theme.colors.navy[5]} />
              <ChevronIcon width={9} height={6} fill={theme.colors.navy[5]} />
            </ProfileButton>
          }
        >
          child
        </Dropdown>
      </RightPane>
    </Wrapper>
  );
};

export default Navbar;
