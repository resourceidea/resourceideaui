import styled from 'styled-components'
import { Box } from "@chakra-ui/core";

export const Wrapper  = styled(Box)``;
export const LeftPane  = styled(Box)``;
export const RightPane  = styled(Box)``;
export const TitleSection  = styled(Box)``;
export const SearchSection  = styled(Box)``;
export const LinkWrapper  = styled(Box)``;
export const NavSectionWrapper  = styled(Box)``;
export const NavTitle  = styled(Box)``;

export const NavLink  = styled(Box)`
    margin-bottom: 0.5rem;
  &:last-of-type {
    margin-top: 0.5rem;
  }
  &:last-of-type {
    margin-bottom: 0px;
  }
  a {
    &>div {
      padding: 0.5rem;
      border-radius: 0.25rem;
      &:hover {
        background-color: rgba(255, 255, 255, 0.05)
      }
    }
    text-decoration: none;
    cursor: pointer;
  }

`;