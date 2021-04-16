import React from 'react';

type IconProps = {
  width?: number
  height?: number,
  fill?: string,
  size?: number
}

export const ResourceIcon = ({ size, width, height, fill }: IconProps) => (
  <svg
    width={size ?? width ?? '13'}
    height={size ?? height ?? '13'}
    viewBox={`0 0 ${size ?? width ?? '13'} ${size ?? height ?? '13'}`}
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
  >
    <path
      d={`M11.5556 13C12.3522 13 13 12.3522 13 11.5556V1.44444C13 0.647833 12.3522 0 
          11.5556 0H1.44444C0.647833 0 0 0.647833 0 1.44444V11.5556C0 12.3522 0.647833
          13 1.44444 13H11.5556ZM4.73272 4.80856L7.62161 6.253L8.74322 4.0105L10.0353 
          4.65689L8.268 8.19217L5.37911 6.74772L4.2575 8.99022L2.96544 8.34383L4.73272 
          4.80856Z`}
      fill={fill ?? 'white'}
    />
  </svg>

);

export default {
  ResourceIcon,
};
