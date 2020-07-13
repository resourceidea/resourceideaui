import React from 'react'
import { Link } from 'react-router-dom'
import { Icon } from "@chakra-ui/core";
import { Box } from "@chakra-ui/core";
import {
  Wrapper,
  LeftPane,
  RightPane,
  TitleSection,
  SearchSection,
  NavSectionWrapper,
  NavTitle,
  LinkWrapper,
  NavLink
} from './Wrappers'
import Text from '../../common/Text'
import Search from '../../common/Search'

const NavSection = ({
  links = [],
  title=''
}) => (
  <NavSectionWrapper padding="1.25rem 1.5rem 1.25rem 1.5rem">
    <NavTitle marginBottom="0.375rem" d="flex" justifyContent="space-between">
      <Text variant="heading6" color="primary.2">{title}</Text>
      <Icon name="arrow" color="primary.3"/>
    </NavTitle>
    <LinkWrapper>
    {links.map(link => (
      <NavLink>
        <Link to={link.url}>
          <Box d="flex" alignItems="center">
            <Icon name={link.icon} color="primary.3" mr="10px"/>
            <Text variant="subtitle1" color="primary.3" key={link.name}>{link.name}</Text>
          </Box>
        </Link>
      </NavLink>
    ))}
    </LinkWrapper>
  </NavSectionWrapper>
)
const Sidebar = () => {
  return (
    <Wrapper d="flex" alignItems="stretch" width="280px" flexShrink="0">
      <LeftPane
        bg="primary.7"
        width="3rem"
        d="flex"
        flexDirection="column"
        alignItems="center"
        pt="1.25rem"
        pb="1.5rem"
        justifyContent="space-between"
      >
        <Box>
          <Icon name="logo" color="neutral.1" size="20px" marginBottom="34px"/>
          <Box
            d="flex" 
            flexDirection="column"
          >
            <Icon name="settings" color="primary.4" margin="12px 0"/>
            <Icon name="people" color="primary.4" margin="12px 0"/>
          </Box>
        </Box>
        <Box>
        <Icon name="help" color="primary.4" />
        </Box>
      </LeftPane>
      <RightPane bg="primary.5" flexGrow="1">
        <TitleSection height="3.5rem" bg="white" opacity="0.1"></TitleSection>
        <SearchSection
          height="4.5rem"
          borderBottomColor="primary.4"
          borderBottomStyle="solid"
          borderBottomWidth="1px"
          pl="1rem"
          pr="1rem"
          d="flex"
          alignItems="center"
        >
          <Search />
        </SearchSection>
        <div>
          <NavSection
            title="Views" 
            links={[
              { url: '#', name: 'Resources Timeline ', icon: 'resources' },
              { url: '#', name: 'Service Lines Timeline ', icon: 'serviceLines' },
              { url: '#', name: 'Job Managers Timeline', icon: 'jobs' },
            ]}/>
          <NavSection
            title="Control Panel" 
            links={[
              { url: '#', name: 'Resources', icon: 'resources' },
              { url: '#', name: 'Clients', icon: 'clients' },
              { url: '#', name: 'Lines of Service', icon: 'serviceLines' },
              { url: '#', name: 'Job Positions', icon: 'jobs' },
            ]}/>
        </div>
      </RightPane>
      
    </Wrapper>
  )
}

export default Sidebar
