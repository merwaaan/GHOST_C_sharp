﻿/*
 * GHOST (General meta-Heuristic Optimization Solving Tool) is a C# library 
 * designed to solve combinatorial satisfaction and optimization problems within 
 * some tenth of milliseconds. It has been originally designed to handle 
 * StarCraft: Brood War-related problems. 
 * 
 * GHOST is a framework aiming to easily model and implement satisfaction and optimization
 * problems. It contains a meta-heuristic solver aiming to solve any kind of these problems 
 * represented by a CSP/COP. It is a generalization of the C++ Wall-in project (https://github.com/richoux/Wall-in) 
 * and a C# adaptation and improvement of the GHOST's C++ version (https://github.com/richoux/GHOST).
 * Please visit https://github.com/richoux/GHOST_C_sharp for further information.
 * 
 * Copyright (C) 2015 Florian Richoux
 *
 * This file is part of GHOST.
 * GHOST is free software: you can redistribute it and/or 
 * modify it under the terms of the GNU General Public License as published 
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.

 * GHOST is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
 * along with GHOST. If not, see http://www.gnu.org/licenses/.
 */

using System.Collections.Generic;
using NUnit.Framework;
using ghost;
using System;

namespace GhostTest
{
  public class VarInt : Variable
  {
    public VarInt( string name, string fullName, Domain domain, int value ) : base( name, fullName, domain, value ) { }
    public VarInt( string name, string fullName ) : base( name, fullName ) { }
  }

  public class SetVariablesWrapper : SetVariables<VarInt>
  {
    public SetVariablesWrapper( List<VarInt> variables ) : base( variables ) { }
    public List<VarInt> GetVariables() { return Variables; }
    public void SetVariables( List<VarInt> variables ) { Variables = variables; }
  }

  [TestFixture]
  public class SetVariablesTest
  {
    public static Domain domain = new Domain( 10, 0 );
    public static SetVariablesWrapper svw = new SetVariablesWrapper( new List<VarInt> { new VarInt( "a", "aa", domain, 5 ), new VarInt( "b", "bb", domain, -1 ) } );

    [TestCase( 3 )]
    [TestCase( 0 )]
    public void GetSizeTest( int size )
    {
      var list = new List<VarInt>();
      for( int i = 0 ; i < size ; ++i )
        list.Add( new VarInt( "", "" ) );

      var svwLocal = new SetVariablesWrapper( list );
      Assert.AreEqual( size, svwLocal.GetNumberVariables() );
    }

    [Test]
    public void GetIndexTest()
    {
      var domain = new Domain( 10, 0 );
      var obj1 = new VarInt( "a", "aa", domain, 5 );
      var obj2 = new VarInt( "b", "bb", domain, -1 );
      var obj3 = new VarInt( "c", "cc", domain, 4 );
      var svw = new SetVariablesWrapper( new List<VarInt> { obj1, obj2 } );

      Assert.AreEqual( 0, svw.GetIndex( obj1 ) );
      Assert.AreEqual( 1, svw.GetIndex( obj2 ) );
      Assert.AreEqual( -1, svw.GetIndex( obj3 ) );
    }

    [Test]
    public void SwapTest()
    {
      svw.Swap( 1, 0 );
      Assert.AreEqual( -1, svw.GetValue( 0 ) );
      Assert.AreEqual( 5, svw.GetValue( 1 ) );
      svw.Swap( 0, 1 );
      Assert.AreEqual( 5, svw.GetValue( 0 ) );
      Assert.AreEqual( -1, svw.GetValue( 1 ) );
    }

    [Test]
    public void ResetDomainTest()
    {
      Assert.AreEqual( 10, svw.Domain( 1 ).GetSize() );

      svw.Domain( 1 ).RemoveValue( 7 );
      svw.Domain( 1 ).RemoveValue( 2 );
      svw.Domain( 1 ).RemoveValue( 4 );

      Assert.AreEqual( 7, svw.Domain( 1 ).GetSize() );

      svw.ResetDomain( 1 );

      Assert.AreEqual( 10, svw.Domain( 1 ).GetSize() );
    }

    [Test]
    public void ResetAllDomainsTest()
    {
      Assert.AreEqual( 10, svw.Domain( 0 ).GetSize() );
      Assert.AreEqual( 10, svw.Domain( 1 ).GetSize() );

      svw.Domain( 0 ).RemoveValue( 7 );
      svw.Domain( 1 ).RemoveValue( 3 );
      svw.Domain( 1 ).RemoveValue( 2 );

      Assert.AreEqual( 9, svw.Domain( 0 ).GetSize() );
      Assert.AreEqual( 8, svw.Domain( 1 ).GetSize() );

      svw.ResetAllDomains();

      Assert.AreEqual( 10, svw.Domain( 0 ).GetSize() );
      Assert.AreEqual( 10, svw.Domain( 1 ).GetSize() );
    }

    [Test]
    public void ShiftUnshiftValueTest()
    {
      Assert.AreEqual( 5, svw.GetValue( 0 ) );
      Assert.AreEqual( -1, svw.GetValue( 1 ) );

      svw.ShiftValue( 0 );
      Assert.AreEqual( 6, svw.GetValue( 0 ) );

      svw.ShiftValue( 1 );
      Assert.AreEqual( -1, svw.GetValue( 1 ) );

      svw.UnshiftValue( 0 );
      Assert.AreEqual( 5, svw.GetValue( 0 ) );

      svw.UnshiftValue( 1 );
      Assert.AreEqual( -1, svw.GetValue( 1 ) );
    }

    [TestCase( 5, 0 )]
    [TestCase( -1, 1 )]
    public void GetValueTest( int value, int index )
    {
      Assert.AreEqual( value, svw.GetValue( index ) );
    }

    [TestCase( -1 )]
    [TestCase( 2 )]
    [ExpectedException(typeof(IndexOutOfRangeException))]
    public void GetValueFailTest( int index )
    {
      svw.GetValue( index );
    }

    [TestCase( "aa", 0 )]
    [TestCase( "bb", 1 )]
    public void FullNameTest( string fullName, int index )
    {
      Assert.AreEqual( fullName, svw.FullName( index ) );
    }
  }
}

