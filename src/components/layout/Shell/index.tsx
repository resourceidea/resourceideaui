import React from "react";
import Navbar from "@/layout/Navbar";
import { Wrapper, Sidebar, Mainbar, ContentBar } from "./Wrappers";
import SideNav from "./SideNav";
import SidePanel from "./SidePanel";

type ShellProps = {
  children?: React.ReactNode;
};

const Shell = ({ children }: ShellProps) => {
  const [open, setOpen] = React.useState(true);
  return (
    <Wrapper>
      <Sidebar open={open}>
        <SidePanel />
        <SideNav open={open} />
      </Sidebar>
      <Mainbar>
        <Navbar setOpen={setOpen} />
        <ContentBar>{children}</ContentBar>
      </Mainbar>
    </Wrapper>
  );
};

export const getLayout = page => (<Shell>{page}</Shell>)

export default Shell;
