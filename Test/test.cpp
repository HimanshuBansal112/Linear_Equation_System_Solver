/*
 * Copyright 2024 Himanshu Bansal
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific
 * language governing permissions and limitations under the License.
 * ==============================================================================
 */
#include "pch.h"
#include <gtest/gtest.h>
#include <vector>
#include "GaussElimination.h"
#include "Fraction_Data_Type.h"
#include "main.h"
#include <unordered_map>

TEST(Solve, Under_determined) {
    GaussEliminationClass test;
    std::vector<std::vector<Fraction>> array = {
        { Fraction(2,1), Fraction(0,1), Fraction(-2,1), Fraction(0,1) },
        { Fraction(0,1), Fraction(2,1), Fraction(-1,1), Fraction(0,1) }
    };
    std::unordered_map<int, Fraction> answer = Solve(test, array);

    ASSERT_TRUE(answer.find(0) != answer.end());
    ASSERT_TRUE(answer.find(1) != answer.end());
    ASSERT_TRUE(answer.find(2) != answer.end());

    EXPECT_EQ(answer[0].ToString(), "1/1");
    EXPECT_EQ(answer[1].ToString(), "1/2");
    EXPECT_EQ(answer[2].ToString(), "1/1");
}

TEST(Solve, Zero_Coefficient_Equation) {
    GaussEliminationClass test;
    std::vector<std::vector<Fraction>> array = {
        { Fraction(0,1), Fraction(1,1), Fraction(-3,1) },
        { Fraction(1,1), Fraction(0,1), Fraction(-2,1) }
    };
    std::unordered_map<int, Fraction> answer = Solve(test, array);

    ASSERT_TRUE(answer.find(0) != answer.end());
    ASSERT_TRUE(answer.find(1) != answer.end());

    EXPECT_EQ(answer[0].ToString(), "2/1");
    EXPECT_EQ(answer[1].ToString(), "3/1");
}

TEST(Solve, Complex_Equation) {
    GaussEliminationClass test;
    std::vector<std::vector<Fraction>> array = {
        { Fraction("1/1"), Fraction("1/2"), Fraction("1/3"), Fraction("1/4"), Fraction("1/5"), Fraction("-5/1")},
        { Fraction("1/2"), Fraction("1/3"), Fraction("1/4"), Fraction("1/5"), Fraction("1/6"), Fraction("-71/20")},
        { Fraction("1/3"), Fraction("1/4"), Fraction("1/5"), Fraction("1/6"), Fraction("1/7"), Fraction("-197/70")},
        { Fraction("1/4"), Fraction("1/5"), Fraction("1/6"), Fraction("1/7"), Fraction("1/8"), Fraction("-657/280")},
        { Fraction("1/5"), Fraction("1/6"), Fraction("1/7"), Fraction("1/8"), Fraction("1/9"), Fraction("-1271/630")}
    };
    std::unordered_map<int, Fraction> answer = Solve(test, array);

    ASSERT_TRUE(answer.find(0) != answer.end());
    ASSERT_TRUE(answer.find(1) != answer.end());
    ASSERT_TRUE(answer.find(2) != answer.end());
    ASSERT_TRUE(answer.find(3) != answer.end());
    ASSERT_TRUE(answer.find(4) != answer.end());

    EXPECT_EQ(answer[0].ToString(), "1/1");
    EXPECT_EQ(answer[1].ToString(), "2/1");
    EXPECT_EQ(answer[2].ToString(), "3/1");
    EXPECT_EQ(answer[3].ToString(), "4/1");
    EXPECT_EQ(answer[4].ToString(), "5/1");
}


TEST(Fraction_Check, HighPrecision) {
    Fraction TemporaryFraction;
    ASSERT_TRUE(Fraction::TryParse("0.16667", TemporaryFraction));
    EXPECT_EQ(TemporaryFraction.ToString(), "1/6");

    ASSERT_TRUE(Fraction::TryParse("0.501", TemporaryFraction));
    EXPECT_EQ(TemporaryFraction.ToString(), "501/1000");

    ASSERT_TRUE(Fraction::TryParse("0.33333", TemporaryFraction));
    EXPECT_EQ(TemporaryFraction.ToString(), "1/3");

    ASSERT_TRUE(Fraction::TryParse("-4697/420", TemporaryFraction));
    EXPECT_EQ(TemporaryFraction.ToString(), "-671/60");

    ASSERT_TRUE(Fraction::TryParse("-20759/2520", TemporaryFraction));
    EXPECT_EQ(TemporaryFraction.ToString(), "-20759/2520");

    ASSERT_TRUE(Fraction::TryParse("-14417/1680", TemporaryFraction));
    EXPECT_EQ(TemporaryFraction.ToString(), "-14417/1680");
    
    ASSERT_TRUE(Fraction::TryParse("-13361/1620", TemporaryFraction));
    EXPECT_EQ(TemporaryFraction.ToString(), "-13361/1620");
}

int main(int argc, char** argv) {
    ::testing::InitGoogleTest(&argc, argv);
    return RUN_ALL_TESTS();
}