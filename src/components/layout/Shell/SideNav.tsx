import React from "react";
import styled from "styled-components";
import Link from "next/link";
import { useRouter } from 'next/router'
import Text from "@/common/Text";
import { ResourceIcon, ServiceIcon, JobIcon, ClientIcon } from '@/common/icons'
import useTheme from '@/lib/hooks/useTheme'

interface SideNavProps {
  open: boolean;
  selected?: boolean;
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
const NavItem = styled.li`
  margin-top: 5px;
  margin-bottom: 5px;
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
    color: ${props => props.theme.colors.neutral[1]};
    background-color:${props => props.theme.colors.teal[6]};
  }
  .navlink-text {
    margin-left: 10px;
  }
`;

const Navlink: React.FC<NavlinkProps> = ({ href, text, icon, open }) => {
  const { asPath } = useRouter();
  const selected = asPath === href;
  return (
    <NavItem style={{ paddingLeft: open ? '0px' : '6px', paddingRight: open ? '0px' : '6px' }}>
      <Link href={href}>
        <AnchorLink open={open} className={selected ? "selected" : ""}>
          {icon}
          {open && <Text variant="body1" className="navlink-text">{text}</Text>}
        </AnchorLink>
      </Link>
    </NavItem>
  )
}

const SideNav: React.FC<SideNavProps> = ({ open }) => {
  const theme = useTheme()
  const iconColor = theme.colors.teal[2]
  return (
    <Wrapper open={open}>
      <TopPanel>{open ? 'Resource Idea' : 'R'}</TopPanel>
      <SearchPanel>{open ? 'Search' : 'S'}</SearchPanel>
      <NavPanel open={open}>
        <Text variant="h4" className="title-text">{open ? 'Timeline Views' : 'T V'}</Text>
        <ul>
          <Navlink open={open} text="Resources" href="/" icon={<ResourceIcon size={13} fill={iconColor} />} />
          <Navlink open={open} text="Service Lines" href="/timeline/service-lines" icon={<ServiceIcon width={13} height={12} fill={iconColor} />} />
          <Navlink open={open} text="Job Managers" href="/timeline/job-managers" icon={<JobIcon width={14} height={13} fill={iconColor} />} />
        </ul>
      </NavPanel>
      <NavPanel open={open}>
        <Text variant="h4" className="title-text">{open ? 'Control Panel' : 'C P'}</Text>
        <ul>
          <Navlink open={open} text="Resources" href="/control-panel/resources" icon={<ResourceIcon size={13} fill={iconColor} />} />
          <Navlink open={open} text="Clients" href="/control-panel/clients" icon={<ClientIcon width={13} height={14} fill={iconColor} />} />
          <Navlink open={open} text="Lines of Service" href="/control-panel/service-lines" icon={<ServiceIcon width={13} height={12} fill={iconColor} />} />
          <Navlink open={open} text="Job Positions" href="/control-panel/job-positions" icon={<JobIcon width={14} height={13} fill={iconColor} />} />
        </ul>
      </NavPanel>
    </Wrapper>
  );
};

export default SideNav;
