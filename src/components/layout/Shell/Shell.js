import React from 'react'
import { Wrapper, MainSection } from './Wrappers'
import Sidebar from '../Sidebar'
import Navbar from '../Navbar'
import Content from '../Content'

const Shell = ({ children }) => {
  return (
    <Wrapper d="flex" minHeight="100vh" width="100vw">
      <Sidebar />
      <MainSection flexGrow="1" overflowX="hidden">
        <Navbar />
        <Content>
          {children}
        </Content>
      </MainSection>
    </Wrapper>
  )
}

export default Shell
