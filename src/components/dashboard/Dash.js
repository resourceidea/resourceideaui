import React from 'react'
import { Box } from '@chakra-ui/core'
import Text from '../common/Text'
import Search from '../common/Search'
import Button from '../common/Button'
import { useSticky } from "react-table-sticky";
import { useTable, useBlockLayout, useResizeColumns } from "react-table";
import {getDashboardColumns, getDashboardData} from "../../data/DashboardRepo";
import Shell from '../layout/Shell'
import { TableWrapper, TopHeaderRow, BottomHeaderRow, StickyCell, OtherCell } from './Wrappers'

const data = getDashboardData();
const columns = getDashboardColumns()

const Table = ({ columns, data }) => {
  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    rows,
    prepareRow
} = useTable({
        columns,
        data
    },
    useBlockLayout,
    useResizeColumns,
    useSticky
    );
    const [topHeaderGroup, bottomHeaderGroup] = headerGroups
  return (
    <TableWrapper ml="12px" borderLeft="1px solid" borderLeftColor="primary.2">
      <Box {...getTableProps()} className='table sticky' maxWidth="100%" height="calc(100vh - (208px))">
          <Box className='header' borderBottom="1px solid" borderBottomColor="primary.2" bg="neutral.1">
            <TopHeaderRow {...topHeaderGroup.getHeaderGroupProps()} className='tr'>
              <Box
                {...topHeaderGroup.headers[0].getHeaderProps()}
                className='th'
                minWidth="196px"
                flexShrink="0"
              >
                  {topHeaderGroup.headers[0].render("Header")}
              </Box>
              <Box
                {...topHeaderGroup.headers[1].getHeaderProps()}
                className='th'
                flexShrink="0"
              >
              {topHeaderGroup.headers[1].render("Header")}
              </Box>
            </TopHeaderRow>
            <BottomHeaderRow {...bottomHeaderGroup.getHeaderGroupProps()} className='tr'>
                {bottomHeaderGroup.headers.slice(0,1).map((column) => (
                  <Box
                    {...column.getHeaderProps()}
                    className='th'
                    minWidth="196px"
                    flexShrink="0"
                  />
                ))}
                {bottomHeaderGroup.headers.slice(1).map((column) => (
                  <Box
                    {...column.getHeaderProps()}
                    className='th'
                    fontSize="9px"
                    width="24px"
                    flexBasis="24px"
                    flexShrink="0"
                  >
                    {column.render("Header")}
                  </Box>
                ))}
            </BottomHeaderRow>
          </Box>
          <div {...getTableBodyProps()} className='body'>
            {rows.map((row) => {
              prepareRow(row);
              return (
                <Box {...row.getRowProps()} className='tr' left="0">
                  {row.cells.slice(0,1).map((cell) => (
                    <StickyCell
                      {...cell.getCellProps()}
                      className='td'
                      bg="neutral.1"
                      minWidth="196px"
                      paddingY="8px"
                      height="50px"
                      flexShrink="0"
                      textTransform="capitalize"
                    >
                      <Box
                      paddingX="14px"
                      paddingY="7px"
                      height="34px"
                      bg="rgba(53, 71, 126, 0.05);"
                      >
                        {cell.render("Cell")}
                      </Box>
                    </StickyCell>
                  ))}
                  {row.cells.slice(1).map((cell) => (
                    <OtherCell
                      {...cell.getCellProps()}
                      className='td'
                      d="flex"
                      paddingY="8px"
                      height="50px"
                      flexBasis="24px"
                      flexShrink="0"
                    >
                      <Box
                        minWidth="24px"
                        position="absolute"
                        top="8px"
                        left="0"
                        height="34px"
                        bg="rgba(53, 71, 126, 0.1)"
                        paddingX="14px"
                        paddingY="7px"
                      >
                        {/* {cell.render("Cell")} */}
                      </Box>
                    </OtherCell>
                  ))}
                </Box>
              );
            })}
          </div>
      </Box>
    </TableWrapper>
  )
}


const Dash = () => {
  return (
    <Shell>
      <Box borderRadius="4px" border="1px solid" borderColor="primary.2" overflow="hidden" bg="neutral.1">
        <Box padding="32px 24px" d="flex" justifyContent="space-between" alignItems="center"  borderBottom="1px solid" borderBottomColor="primary.2">
          <Box d="flex" alignItems="center">
            <Text variant="heading6" color="text.1" width="188px">Resources</Text>
            <Search
              bg="primary.1"
              color="text.2"
              searchColor="text.2"
              focus={{
                borderColor: 'text.2',
                color:"text.2"
              }} />
          </Box>
            <Button variant="primary">
              Assign Job
            </Button>
        </Box>
        <Table columns={columns} data={data} />
      </Box>
    </Shell>
  )
}

export default Dash
