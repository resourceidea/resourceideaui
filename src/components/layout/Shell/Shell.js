import React from 'react'
import { Wrapper, MainSection } from './Wrappers'
import Sidebar from '../Sidebar'
import Navbar from '../Navbar'
import Content from '../Content'

const Shell = ({ children }) => {
  let navSections = [
    {
      title: 'Timelines',
      links:[
        { url: '/dashboard', name: 'Resources ', icon: 'resources' },
        { url: '#', name: 'Service Lines ', icon: 'serviceLines' },
        { url: '#', name: 'Job Managers', icon: 'jobs' },
      ]
    },
    {
      title: 'Control Panel',
      links: [
        { url: '#', name: 'Resources', icon: 'resources' },
        { url: '#', name: 'Clients', icon: 'clients' },
        { url: '#', name: 'Lines of Service', icon: 'serviceLines' },
        { url: '#', name: 'Job Positions', icon: 'jobs' },
      ]
    }
  ]
  return (
    <Wrapper d="flex" minHeight="100vh" width="100vw">
      <Sidebar sections={navSections}/>
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
