import React from "react";
import styled from "styled-components";
import Link from "next/link";
import Text from "@/common/Text";
import { ResourceIcon, ServiceIcon, JobIcon, ClientIcon } from '@/common/icons'
import useTheme from '@/lib/hooks/useTheme'

interface SideNavProps {
  open: boolean;
}
interface NavlinkProps extends SideNavProps {
  href: string;
  icon: React.ReactElement;
  text: string;
}

const Wrapper = styled.div`
  color: white;
  flex-grow:1;
  width: ${(props: SideNavProps) => props.open ? 'unset' : '48px'};
`;
const TopPanel = styled.div`
  height: 57px;
  padding-left: 24px;
  padding-right: 24px;
  display: flex;
  align-items: center;
`;
const SearchPanel = styled.div`
  padding: 24px 20px;
`;
const NavPanel = styled.div`
  padding-left: ${(props: SideNavProps) => props.open ? '24px' : '0px'};
  padding-right: ${(props: SideNavProps) => props.open ? '24px' : '0px'};
  padding-top: 12px;
  ul {
    padding: 0;
    list-style: none;
  }
  .title-text {
    text-align: ${(props: SideNavProps) => props.open ? 'unset' : 'center'};
  }
`;


const AnchorLink = styled.a`
  cursor: pointer;
  border-radius: 4px;
  width: 100%;
  text-decoration: none;
  padding: ${(props: SideNavProps) => props.open ? '10px' : '10px 0px'};
  display: flex;
  justify-content: ${(props: SideNavProps) => props.open ? 'unset' : 'center'};
  color: ${props => props.theme.colors.teal[2]};
  align-items: center;
  &:hover {
    color: ${props => props.theme.colors.teal[1]};
    background-color:${props => props.theme.colors.teal[6]};
    svg {
      fill: ${props => props.theme.colors.teal[2]};
    }
  }
  &.selected {
  background-color:${props => props.theme.colors.teal[6]};
  }
  .navlink-text {
    margin-left: 10px;
  }
`;

const Navlink: React.FunctionComponent<NavlinkProps> = ({ href, text, icon, open }) => {
  return (
    <li style={{ paddingLeft: open ? '0px' : '6px', paddingRight: open ? '0px' : '6px' }}>
      <Link href={href}>
        <AnchorLink open={open}>
          {icon}
          {open && <Text variant="body1" className="navlink-text">{text}</Text>}
        </AnchorLink>
      </Link>
    </li>
  )
}

const SideNav: React.FunctionComponent<SideNavProps> = ({ open }) => {
  const theme = useTheme()
  const iconColor = theme.colors.teal[2]
  return (
    <Wrapper open={open}>
      <TopPanel>{open ? 'Resource Idea' : 'R'}</TopPanel>
      <SearchPanel>{open ? 'Search' : 'S'}</SearchPanel>
      <NavPanel open={open}>
        <Text variant="h4" className="title-text">{open ? 'Timeline Views' : 'T V'}</Text>
        <ul>
          <Navlink open={open} text="Resources" href="#" icon={<ResourceIcon size={13} fill={iconColor} />} />
          <Navlink open={open} text="Service Lines" href="#" icon={<ServiceIcon width={13} height={12} fill={iconColor} />} />
          <Navlink open={open} text="Job Managers" href="#" icon={<JobIcon width={14} height={13} fill={iconColor} />} />
        </ul>
      </NavPanel>
      <NavPanel open={open}>
        <Text variant="h4" className="title-text">{open ? 'Control Panel' : 'C P'}</Text>
        <ul>
          <Navlink open={open} text="Resources" href="#" icon={<ResourceIcon size={13} fill={iconColor} />} />
          <Navlink open={open} text="Service Lines" href="#" icon={<ClientIcon width={13} height={14} fill={iconColor} />} />
          <Navlink open={open} text="Lines of Service" href="#" icon={<ServiceIcon width={13} height={12} fill={iconColor} />} />
          <Navlink open={open} text="Job Positions" href="#" icon={<JobIcon width={14} height={13} fill={iconColor} />} />
        </ul>
      </NavPanel>
    </Wrapper>
  );
};

export default SideNav;
