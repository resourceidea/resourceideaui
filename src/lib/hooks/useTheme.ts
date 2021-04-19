import { useContext } from 'react';
import { ThemeContext } from 'styled-components';

const useTheme = () => {
  return useContext(ThemeContext)
}

export default useTheme;