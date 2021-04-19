import React from "react";
import styled from "styled-components";
import Link from "next/link";
import Text from "@/common/Text";

const Wrapper = styled.div`
  color: white;
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
  padding-left: 24px;
  padding-right: 24px;
  padding-top: 12px;
  ul {
    padding: 0;
    list-style: none;
  }
`;

const SideNav = () => {
  return (
    <Wrapper>
      <TopPanel>Resourcce Idea</TopPanel>
      <SearchPanel>Search</SearchPanel>
      <NavPanel>
        <Text variant="h4">Timeline Views</Text>
        <ul>
          <li>
            <Link href="#">
              <a>
                <Text variant="p">Resources</Text>
              </a>
            </Link>
          </li>
          <li>
            <Link href="#">
              <a>
                <Text variant="p">Service Lines</Text>
              </a>
            </Link>
          </li>
          <li>
            <Link href="#">
              <a>
                <Text variant="p">Job Managers</Text>
              </a>
            </Link>
          </li>
        </ul>
      </NavPanel>
      <NavPanel>
        <Text variant="h4">Control Panel</Text>
        <ul>
          <li>
            <Link href="#">
              <a>
                <Text variant="p">Resources</Text>
              </a>
            </Link>
          </li>
          <li>
            <Link href="#">
              <a>
                <Text variant="p">Clients</Text>
              </a>
            </Link>
          </li>
          <li>
            <Link href="#">
              <a>
                <Text variant="p">Lines of Service</Text>
              </a>
            </Link>
          </li>
          <li>
            <Link href="#">
              <a>
                <Text variant="p">Job Positions</Text>
              </a>
            </Link>
          </li>
        </ul>
      </NavPanel>
    </Wrapper>
  );
};

export default SideNav;
