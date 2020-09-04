import React from 'react'
import { Box } from '@chakra-ui/core'
import dateFormat from 'dateformat';
export const getMonthsInRange = (range = {
    start: {
      month: new Date().getMonth(),
      year: new Date().getFullYear()
    },
    end: {
      month: new Date().getMonth() + 4,
      year: new Date().getFullYear()

    }
  }) => {
    let months = []
    let currentYear = range.start.year
    while((range.end.year - currentYear) > -1) {
      let firstMonth = currentYear === range.start.year ? range.start.month : 0
      let lastMonth = currentYear === range.end.year ? range.end.month : 11
      for (let x = firstMonth; x <= lastMonth; x++) {
        const days = new Date(currentYear, x + 1, 0).getDate() // calculates next month's first date - 1
        months.push({ month: x, days, year: currentYear })
      }
      currentYear += 1
    }
    return months
  }

  const getDays = (date1, date2) => {
    let startDate = new Date(dateFormat(date1, "fullDate"))
    let endDate = new Date(dateFormat(date2, "fullDate"))
    let timeDiff = endDate.getTime() - startDate.getTime()
    console.log(`${timeDiff / (1000 * 3600 * 24)} days`)
    return timeDiff / (1000 * 3600 * 24)
  }
  export function getDashboardColumns() {
    let monthHeaders = getMonthsInRange().map(({ month, year, days }) => {
      const monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
      const dayNames = ['S', 'M', 'T', 'W', 'T', 'F', 'S']
      return {
        Header: monthNames[month],
        accessor: '',
        columns: Array(days).fill().map((undef,i) => {
          let day = i+1
          let dayLiteral = `${day}`.split('').length === 1 ? `0${day}`: day
          let  date = new Date(year, month, day)
          let dayName = dayNames[date.getDay()]
        return ({
            Header: dayLiteral,
            width: 28,
            isLastDayOfMonth: day === days,
            isWeekend: dayName === 'S',
            Cell: (cell) => (cell.value.map(assignment => {
              return (
              dateFormat(assignment.start_date, "fullDate") === dateFormat(date, "fullDate") ? (
              <Box
                key={date}
                position="absolute"
                top="0px"
                padding="6px 16px"
                zIndex="2"
                height="34px"
                whiteSpace="nowrap"
                overflow="hidden"
                textOverflow="ellipsis"
                fontSize="13px"
                display="flex"
                alignItems="center"
                color="white"
                background={assignment.engagement.color ||"teal"}
                left="0px"
                borderRadius="2px"
                width={`${getDays(date, assignment.end_date) * 24}px`}
              >
                {assignment.engagement.title}
              </Box>
              ) : null
              )
            })),
            accessor: row => row.assignments,
            id: `${year}-${monthNames[month]}-${day}`,
          })
      })
      }
    })
    return [
      {
        Header: "Showing",
        sticky: 'left',
        columns: [
          {
            Header: "Left Column",
            accessor: "fullname",
          }
        ]
      },
      ...monthHeaders
    ];
  }
 const names = ["John Opio", "Brian Kazibwe", "Agnes Natasha", "Brenda Kimuli", "Ashraf Tugume"]
  const dates = ['08', '10', '09']
  const dd = ['01', '06', '02']
  const colors = ['teal', '#4965B6', '#F2994A', '#9B51E0', '#56CCF2']
  export const getDashboardData = () => {
      return (
         Array(12).fill().map(datum => {
           let item = {
            fullname: names[Math.floor(Math.random() * 5)],
            utilization: "49",
            job_position: "Associate Auditor",
            link_url: "https://api.resourceidea.com/employees/12",
            assignments: 
            [
              {
                client: {
                  name: 'Client name',
                  link_url: 'https://api.resourceidea.com/clients/1'
                },
                engagement: {
                  title: 'Engagement title',
                  link_url: 'https://api.resourceidea.com/engagements/1',
                  color: colors[Math.floor(Math.random() * 5)],
                  manager: {
                    fullname: 'Mwanga Muteesa'
                  },
                  partner: {
                    fullname: 'Kabalega Rukidi'
                  },
                  assignees: [
                    {
                      fullname: 'Nalubaale Jinja',
                      job_position: 'Senior Auditor'
                    },
                    {
                      fullname: 'Gulu Nambi',
                      job_position: 'Associate Auditor'
                    }
                  ]
                },
                start_date: `2020-08-${dd[Math.floor(Math.random() * 3)]}T08:40:51.620Z`,
                end_date: `2020-${dates[Math.floor(Math.random() * 3)]}-08T08:40:51.620Z`
              }
            ]
            }
            return item
          })
       )
    }