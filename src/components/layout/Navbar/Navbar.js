import React from 'react'
import { Wrapper } from './Wrappers'
import { Icon } from "@chakra-ui/core";

const Navbar = () => {
  return (
    <Wrapper
      borderBottomColor="primary.2"
      borderBottomWidth="1px"
      borderBottomStyle="solid"
      height="3.5rem"
      px="1.25rem"
      d="flex"
      alignItems="center"
      justifyContent="space-between"
    >
      <Icon color="text.1" name="menu" cursor="pointer" />
      Navbar
    </Wrapper>
  )
}

export default Navbar
