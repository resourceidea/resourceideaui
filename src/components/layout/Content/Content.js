import React from 'react'
import { Wrapper } from './Wrappers'

const Content = ({ children }) => {
  return (
    <Wrapper
      minHeight="calc(100vh - (3.5rem + 1px))"
      bg="neutral.2"
      pt="1.5rem"
      px="1.25rem"
      className="content"
      width="100%"
    >
      {children}
    </Wrapper>
  )
}

export default Content
