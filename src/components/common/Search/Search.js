import React from 'react'
import { InputGroup, InputLeftElement, Input, Icon } from '@chakra-ui/core'

const Search = ({
  bg="rgba(255,255,255, 0.13)",
  color="primary.2",
  searchColor="primary.3",
  focus= {
    borderColor: 'primary.3',
    color:"primary.1"
  }
}) => {
  return (
  <InputGroup>
    <InputLeftElement
      children={
      <Icon name="search" color={searchColor} size="14px" />
      }
      width="3rem"
      height="2rem"
      />
    <Input
      type="text"
      placeholder="Search"
      bg={bg}
      border="none"
      color={color}
      borderRadius="512px"
      height="2rem"
      fontSize="0.75rem"
      _focus={{
        border: "1px solid",
        ...focus
      }}
    />
  </InputGroup>
  )
}

export default Search
