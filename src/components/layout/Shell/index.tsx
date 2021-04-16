import React from 'react';
import Navbar from '@/common/Navbar'
import { Wrapper, Sidebar, SidePanel, Mainbar, ContentBar } from './Wrappers'
import SideNav from './SideNav'

type ShellProps = {
  children?: React.ReactNode
}

const Shell = ({ children }: ShellProps) => {
  const [open, setOpen] = React.useState(true)
  return (
    <Wrapper>
      <Sidebar open={open}>
        <SidePanel />
        <SideNav />
      </Sidebar>
      <Mainbar>
        <Navbar setOpen={setOpen} />
        <ContentBar>
          {children}
        </ContentBar>
      </Mainbar>
    </Wrapper>
  )
};

export default Shell;
