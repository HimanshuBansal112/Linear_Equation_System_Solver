cmake_minimum_required(VERSION 3.20)

project(LinearEquationSolver)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

include_directories(${CMAKE_SOURCE_DIR})

file(GLOB SOURCES "*.cpp")

add_executable(LinearEquationSolver ${SOURCES})