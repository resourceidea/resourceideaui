import React, { useMemo, Fragment, useRef, useState } from 'react'
import { Box } from '@chakra-ui/core'
import Text from '../common/Text'
import Search from '../common/Search'
import Button from '../common/Button'
import { useSticky } from "react-table-sticky";
import { useTable, useBlockLayout, useResizeColumns } from "react-table";
import {getDashboardColumns, getDashboardData} from "../../data/DashboardRepo";
import Shell from '../layout/Shell'
import { TableWrapper, TopHeaderRow, BottomHeaderRow, StickyCell, OtherCell, DateBox } from './Wrappers'

const data = getDashboardData();
const columns = getDashboardColumns()

const Table = ({ columns, data }) => {
  const pickAMonth = useRef(null)
  const rangeValue = useState({ from: { year: 2020, month: 8 }, to: { year: 2020, month: 9 }})

  const handleAMonthChange = (value) => {
    console.log('value', value)
  }
  const handleAMonthDissmis = (value) => {
    console.log('value', value)
  }
  const handleClickMonthBox = (value) => {
    console.log('value', value)
  }
  const pickerLang = {
    months: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    from: 'From', to: 'To',
  }
  const tableColumns = useMemo(() => columns, [columns])
  const tableData = useMemo(() => data, [data])
  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    rows,
    prepareRow
} = useTable({
      columns: tableColumns,
      data: tableData
    },
    useBlockLayout,
    useResizeColumns,
    useSticky
    );
    const [topHeaderGroup, bottomHeaderGroup] = headerGroups
    console.log(topHeaderGroup.headers.slice(1))
    const endOfMonthColor = 'maroon.4'
    const endOfMonthBorderWidth = '1px solid'
  return (
    <TableWrapper ml="12px" borderLeft="1px solid" borderLeftColor="primary.2">
      <Box {...getTableProps()} className='table sticky' maxWidth="100%" height="calc(100vh - (208px))">
          <Box className='header' borderBottom="1px solid" borderBottomColor="primary.2" bg="neutral.1">
            <TopHeaderRow {...topHeaderGroup.getHeaderGroupProps()} className='tr'>
              <Box
                {...topHeaderGroup.headers[0].getHeaderProps()}
                className='th'
                position="relative"
                minWidth="210px"
                flexShrink="0"
              >
                <Box
                  height="38px"
                  backgroundColor="white"
                  display="flex"
                  justifyContent="space-between"
                  alignItems="center"
                  paddingX="8px"
                  fontSize="13px"
                  position="absolute"
                  top="0"
                  width="100%"
                  zIndex="4"
                  left="0"
                >
                  {topHeaderGroup.headers[0].render("Header")}
                  <Box
                    borderRadius="4px"
                    backgroundColor="navy.1"
                    fontWeight="bold"
                    fontSize="12px"
                    padding="4px 10px"
                    display="flex"
                    alignItems="center"
                  > - 
                  </Box>
                </Box>
              </Box>
              {topHeaderGroup.headers.slice(1).map((column) => (
                <Box
                  {...column.getHeaderProps()}
                  className='th'
                  paddingLeft="8px"
                  flexShrink="0"
                  paddingTop="4px"
                  fontSize="11px"
                  textTransform="uppercase"
                  fontWeight="bold"
                  color="maroon.4"
                  borderRight={endOfMonthBorderWidth}
                  borderRightColor={endOfMonthColor}
                >
                {column.render("Header")}
                </Box>
              ))}
            </TopHeaderRow>
            <BottomHeaderRow {...bottomHeaderGroup.getHeaderGroupProps()} className='tr'>
                {bottomHeaderGroup.headers.slice(0,1).map((column) => (
                  <Box
                    {...column.getHeaderProps()}
                    className='th'
                    background="transparent"
                    zIndex="0"
                    backgroundColor="transparent !important"
                    pointerEvents="none"
                    minWidth="210px"
                    flexShrink="0"
                  />
                ))}
                {bottomHeaderGroup.headers.slice(1).map((column) => (
                  <Box
                    {...column.getHeaderProps()}
                    className='th'
                    paddingTop="4px"
                    fontSize="9px"
                    textAlign="center"
                    color={column.isWeekend ? "teal.5" : "primary.7" }
                    width="28px"
                    fontWeight="700"
                    flexBasis="28px"
                    flexShrink="0"
                    height="20px"
                    borderRight={column.isLastDayOfMonth ? endOfMonthBorderWidth : 'none'}
                    borderRightColor={column.isLastDayOfMonth ? endOfMonthColor : 'unset'}
                  >
                    <div>{column.render("Header")}</div>
                  </Box>
                ))}
            </BottomHeaderRow>
          </Box>
          <div {...getTableBodyProps()} className='body'>
            {rows.map((row, ri) => {
              prepareRow(row);
              return (
                <Box {...row.getRowProps()} className='tr' left="0">
                  {row.cells.slice(0,1).map((cell) => (
                    <StickyCell
                      {...cell.getCellProps()}
                      className='td'
                      bg="neutral.1"
                      minWidth="210px"
                      paddingY="8px"
                      height="50px"
                      flexShrink="0"
                      textTransform="capitalize"
                    >
                      <Box
                      paddingX="14px"
                      paddingY="7px"
                      height="34px"
                      fontSize="13px"
                      bg="rgba(53, 71, 126, 0.05);"
                      >
                        {cell.render("Cell")}
                      </Box>
                    </StickyCell>
                  ))}
                  {console.log('??', row)}
                  {row.cells.slice(1).map((cell, i) => (
                    <Fragment key={cell.column.id}>
                    <OtherCell
                      {...cell.getCellProps()}
                      className='td'
                      isWeekend={cell.column.isWeekend}
                      borderRight={cell.column.isLastDayOfMonth ? endOfMonthBorderWidth : 'none'}
                      borderRightColor={cell.column.isLastDayOfMonth ? endOfMonthColor : 'unset'}
                      d="flex"
                      paddingY="8px"
                      height="50px"
                      flexBasis="24px"
                      flexShrink="0"
                      width="24px"
                      overflow="visible"
                    >
                      <Box
                        width="24px"
                        left="0"
                        height="34px"
                        bg="rgba(53, 71, 126, 0.1)"
                        paddingX="14px"
                        paddingY="7px"
                        position="relative"
                      >
                      {cell.render("Cell")}
                      </Box>
                    </OtherCell>
                    </Fragment>
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
